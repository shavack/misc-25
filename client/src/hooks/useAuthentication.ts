import { useState } from "react";
import { authenticateUser } from "../api/authentication";
import { useAuth } from "../contexts/AuthContext";

export const useAuthentication = () => {
  const auth = useAuth();
  const [isLoading, setLoading] = useState(false)
  const [error, setError] = useState<Error | null>(null)

  const authenticate = async (username: string, password: string) => {
    setLoading(true)
    setError(null)
    try {
      const token = await authenticateUser(username, password);
      if (auth) {
        auth.login(token);
      } else {
        throw new Error("Authentication context is not available.");
      }
    } catch (err: any) {
      setError(err)
      throw err
    } finally {
      setLoading(false)
    }
  };

  return { authenticate, isLoading, error };
};