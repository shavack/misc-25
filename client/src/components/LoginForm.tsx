import { useState } from "react"
import { useAuthentication } from "../hooks/useAuthentication"

export default function LoginForm() {
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const { authenticate, isLoading, error } = useAuthentication()

  const handleSubmit = async (e: { preventDefault: () => void }) => {
    e.preventDefault()
    try {
      await authenticate(username, password)
      console.log("Login successful")
    }
    catch { }
    finally {
      setUsername("")
      setPassword("")
    }
      
  }

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-4">
      <input
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        className="p-2 border rounded"
        disabled={isLoading}
      />
      <input
        placeholder="Password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        className="p-2 border rounded"
        disabled={isLoading}
      />
      <button type="submit" className="bg-blue-500 text-white p-2 rounded" disabled={isLoading}>
        {isLoading ? "Logging in..." : "Log in"}
      </button>
      {error && <p className="text-red-500">{error.message}</p>}
    </form>
  )
}
