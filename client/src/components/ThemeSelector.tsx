import { useTheme } from '../contexts/ThemeContext'
import { themes } from '../themes'

export default function ThemeSelector() {
  const { theme, setTheme } = useTheme()

  return (
    <select
      className="p-2 rounded-md"
      value={theme}
      onChange={(e) => setTheme(e.target.value as keyof typeof themes)}
    >
      {Object.keys(themes).map((key) => (
        <option key={key} value={key}>
          {key}
        </option>
      ))}
    </select>
  )
}
