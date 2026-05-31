import { useEffect, useMemo, useState } from "react";
import { MUSCLE_GROUPS } from "@/shared/constants/muscleGroups";

const SEARCH_DEBOUNCE_MS = 400;

export function useEquipmentFiltering(items) {
  const [searchInput, setSearchInput] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");

  useEffect(() => {
    const timer = setTimeout(() => setDebouncedSearch(searchInput), SEARCH_DEBOUNCE_MS);

    return () => clearTimeout(timer);
  }, [searchInput]);

  const filtered = useMemo(() => {
    const q = debouncedSearch.trim().toLowerCase();
    
    if (!q) return items;
    
    return items.filter((e) => e.name.toLowerCase().includes(q));
  }, [items, debouncedSearch]);

  const grouped = useMemo(() => {
    if (debouncedSearch.trim()) return null;
    
    return MUSCLE_GROUPS.reduce((acc, g) => {
      const group = items.filter((e) => e.muscleGroup === g.value);
      
      if (group.length > 0) acc[g.value] = group;
      
      return acc;
    }, {});
  }, [items, debouncedSearch]);

  return {
    filtered,
    grouped,
    searchInput,
    setSearchInput,
    isSearching: !!debouncedSearch.trim(),
  };
}
