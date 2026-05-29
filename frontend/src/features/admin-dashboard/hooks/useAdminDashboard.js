import { useState, useEffect } from "react";
import { toast } from "sonner";
import { getAdminStats, getAdminCoaches } from "../services/adminDashboardService";

export function useAdminDashboard() {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState(null);
  const [coaches, setCoaches] = useState([]);
  const [totalCoaches, setTotalCoaches] = useState(0);

  useEffect(() => {
    async function load() {
      setLoading(true);

      try {
        const [statsData, coachesData] = await Promise.all([
          getAdminStats(),
          getAdminCoaches(1, 8),
        ]);

        if (statsData) setStats(statsData);
        
        if (coachesData) {
          setCoaches(coachesData.coaches ?? []);
          setTotalCoaches(coachesData.totalCoaches ?? 0);
        }
      } catch (error) {
        toast.error(error?.message || "Erro ao carregar o dashboard.");
      } finally {
        setLoading(false);
      }
    }
    
    load();
  }, []);

  return { loading, stats, coaches, totalCoaches };
}
