import { createContext, useContext, useState, type ReactNode } from 'react'
import { themes, type ThemeName } from '../themes'

type ThemeContextType = {
  theme: ThemeName
  setTheme: (name: ThemeName) => void
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined)

export const ThemeProvider = ({ children }: { children: ReactNode }) => {
  const [theme, setTheme] = useState<ThemeName>('pastel')
  return (
    <ThemeContext.Provider value={{ theme, setTheme }}>
      <div className={`${themes[theme].background} min-h-screen`}>
        {children}
      </div>
    </ThemeContext.Provider>
  )
}

export const useTheme = () => {
  const ctx = useContext(ThemeContext)
  if (!ctx) throw new Error('useTheme must be used within ThemeProvider')
  return ctx
}