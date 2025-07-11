import axios from "axios";
import { API_BASE_URL } from "../constants/constants";

export const authenticateUser = async (username: string, password: string): Promise<string> => {
  try {
    const response = await axios.post(`${API_BASE_URL}/login`, {
      username,
      password,
    });
    return response.data;
  } catch (error) {
    throw new Error("Invalid username or password");
  }
}
export const registerUser = async (username: string, password: string): Promise<void> => {
  try {
    await axios.post(`${API_BASE_URL}/register`, {
      username,
      password,
    });
  } catch (error) {
    throw new Error("Registration failed");
  }
};
