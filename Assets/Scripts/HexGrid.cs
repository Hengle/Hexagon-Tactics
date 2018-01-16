using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class HexGrid : MonoBehaviour {

	public int cellCountX = 8, cellCountZ = 8;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;
	public HexGridChunk chunkPrefab;

	public Texture2D noiseSource;

	public int seed;

	HexGridChunk[] chunks;
	HexCell[] cells;

    HexCellPriorityQueue searchFrontier;

    int chunkCountX, chunkCountZ;

    HexCellShaderData cellShaderData;

    void Awake () {
		HexMetrics.noiseSource = noiseSource;
		HexMetrics.InitializeHashGrid(seed);
        cellShaderData = gameObject.AddComponent<HexCellShaderData>();
        CreateMap(cellCountX, cellCountZ);
	}

	public bool CreateMap (int x, int z) {
		if (
			x <= 0 || x % HexMetrics.chunkSizeX != 0 ||
			z <= 0 || z % HexMetrics.chunkSizeZ != 0
		) {
			Debug.LogError("Unsupported map size.");
			return false;
		}

		if (chunks != null) {
			for (int i = 0; i < chunks.Length; i++) {
				Destroy(chunks[i].gameObject);
			}
		}

		cellCountX = x;
		cellCountZ = z;
		chunkCountX = cellCountX / HexMetrics.chunkSizeX;
		chunkCountZ = cellCountZ / HexMetrics.chunkSizeZ;
        cellShaderData.Initialize(cellCountX, cellCountZ);
        CreateChunks();
		CreateCells();
		return true;
	}

	void CreateChunks () {
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++) {
			for (int x = 0; x < chunkCountX; x++) {
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateCells () {
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++) {
			for (int x = 0; x < cellCountX; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void OnEnable () {
		if (!HexMetrics.noiseSource) {
			HexMetrics.noiseSource = noiseSource;
			HexMetrics.InitializeHashGrid(seed);
//			HexMetrics.colors = colors;
		}
	}

    public List<HexCell> GetCells() {
        return cells.ToList<HexCell>();
    }

	public HexCell GetCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index =
			coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        if (index > cells.Length) return null;
		return cells[index];
	}

	public HexCell GetCell (HexCoordinates coordinates) {
		int z = coordinates.Z;
		if (z < 0 || z >= cellCountZ) {
			return null;
		}
		int x = coordinates.X + z / 2;
		if (x < 0 || x >= cellCountX) {
			return null;
		}
		return cells[x + z * cellCountX];
	}

    public List<HexCell> SearchInRange(HexCell fromCell, int range, bool includeStart) {
        List<HexCell> retCells = ListPool<HexCell>.Get();
        if (fromCell == null) return retCells;

        if (searchFrontier == null) {
            searchFrontier = new HexCellPriorityQueue();
        }
        else {
            searchFrontier.Clear();
        }

        ClearSearch();

        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);
        while (searchFrontier.Count > 0) {
            HexCell current = searchFrontier.Dequeue();
            current.SearchPhase = 1;

                retCells.Add(current);

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || neighbor.SearchPhase > 0) {
                    continue;
                }
                if (neighbor.IsUnderwater) {
                    continue;
                }
                if (current.GetEdgeType(neighbor) == HexEdgeType.Cliff) {
                    continue;
                }

                int distance = current.Distance + 1;
                if (distance > range) {
                    continue;
                }

                if (neighbor.SearchPhase == 0) {
                    neighbor.SearchPhase = 1;
                    neighbor.Distance = distance;
                    neighbor.SearchHeuristic = 0;
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance) {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    searchFrontier.Change(neighbor, oldPriority);
                }

                
            }
        }
        if (!includeStart)
            retCells.Remove(fromCell);
        return retCells;
    } 

    public List<HexCell> FindPath(HexCell fromCell, HexCell toCell) {
        return Search(fromCell, toCell);
    }

    /*List<HexCell> BaseSearch(HexCell fromCell, Func<HexCell, HexCell, bool> expand) {
        List<HexCell> cells = new List<HexCell>();
       cells.Add(fromCell);

        ClearSearch();

    } */

    List<HexCell> Search(HexCell fromCell, HexCell toCell) {
        List<HexCell> path = new List<HexCell>();
        if (fromCell == toCell) {
            return null;
        }  
        if (searchFrontier == null) {
            searchFrontier = new HexCellPriorityQueue();
        }
        else {
            searchFrontier.Clear();
        }

        for (int i = 0; i < cells.Length; i++) {
            cells[i].Distance = int.MaxValue;
            cells[i].PathFrom = null;
        }

        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);
        while (searchFrontier.Count > 0) {
            HexCell current = searchFrontier.Dequeue();
            if (current == toCell) {
                path.Add(current);
                current = current.PathFrom;
                while (current != fromCell) {
                    path.Add(current);
                    current = current.PathFrom;
                }
                break;
            }
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null) {
                    continue;
                }
                if (neighbor.IsUnderwater) {
                    continue;
                }
                if (current.GetEdgeType(neighbor) == HexEdgeType.Cliff) {
                    continue;
                }
                int distance = current.Distance + 1;

                if (neighbor.Distance == int.MaxValue) {
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic =
                        neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance) {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }             
            }
        }
        return path;
    }

    void ClearSearch() {
        foreach (HexCell c in cells) {
            c.SearchPhase = 0;
            c.PathFrom = null;
            c.Distance = int.MaxValue;
            c.SetLabel("");
        }
    }

    public void UnHighlightAllCells() {
        for (int i = 0; i < cells.Length; i++) {
            cells[i].DisableHighlight();
            //cells[i].DecreaseVisibility();
        }
    }

    public void HighlightCells(List<HexCell> c, Color color) {
        foreach (HexCell cell in c) {
            cell.EnableHighlight(color);
            //cell.IncreaseVisibility();
        }
    }

    public void UnHighlightCells(List<HexCell> c) {
        foreach (HexCell cell in c) {
            cell.DisableHighlight();
            //cell.DecreaseVisibility();
        }
    }

    public void HighlightCell(HexCell c, Color color) {
        c.EnableHighlight(color);
        
    }

    public void ShowUI (bool visible) {
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].ShowUI(visible);
		}
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.Index = i;
        cell.ShaderData = cellShaderData;

        if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		cell.uiRect = label.rectTransform;

		cell.Elevation = 0;

		AddCellToChunk(x, z, cell);
	}

	void AddCellToChunk (int x, int z, HexCell cell) {
		int chunkX = x / HexMetrics.chunkSizeX;
		int chunkZ = z / HexMetrics.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * HexMetrics.chunkSizeX;
		int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
	}

	public void Save (BinaryWriter writer) {
		writer.Write(cellCountX);
		writer.Write(cellCountZ);

		for (int i = 0; i < cells.Length; i++) {
			cells[i].Save(writer);
		}
	}

	public void Load (BinaryReader reader, int header) {
        StopAllCoroutines();
        int x = 20, z = 15;
		if (header >= 1) {
			x = reader.ReadInt32();
			z = reader.ReadInt32();
		}
		if (x != cellCountX || z != cellCountZ) {
			if (!CreateMap(x, z)) {
				return;
			}
		}

		for (int i = 0; i < cells.Length; i++) {
			cells[i].Load(reader);
		}
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].Refresh();
		}
	}
}