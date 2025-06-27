import { createContext, useContext, useState, type ReactNode } from 'react'
import { themes } from '../themes'

type ThemeName = keyof typeof themes

type ThemeContextType = {
  theme: ThemeName
  setTheme: (name: ThemeName) => void
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined)

export const ThemeProvider = ({ children }: { children: ReactNode }) => {
  const [theme, setTheme] = useState<ThemeName>('dark')
  return (
    <ThemeContext.Provider value={{ theme, setTheme }}>
      <div className={`${themes[theme].background} min-h-screen w-full`}>
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