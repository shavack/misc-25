import { useTheme } from '../contexts/ThemeContext'
import { themes } from '../themes'

export default function ThemeSelector() {
  const { theme, setTheme } = useTheme()
  const currentTheme = themes[theme];
  return (
    <>
    <div>
      <label htmlFor="title" className={`text-sm font-medium mb-1 ${currentTheme.background} ${currentTheme.text}`}>
        Theme 
      </label>
    </div>
    <select
      className={`p-2 rounded-md ${currentTheme.background} ${currentTheme.text}`}
      value={theme}
      onChange={(e) => setTheme(e.target.value as keyof typeof themes)}
    >
      {Object.keys(themes).map((key) => (
        <option key={key} value={key}>
          {key}
        </option>
      ))}
    </select>
    </>
  )
}
