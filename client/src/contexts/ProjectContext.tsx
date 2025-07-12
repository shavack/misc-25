import { createContext, useContext, useState, type ReactNode } from "react"

type ProjectContextType = {
  currentProjectID: number
  setCurrentProjectID: (id: number) => void
}

const ProjectContext = createContext<ProjectContextType | undefined>(undefined)

export const ProjectProvider = ({ children }: { children: ReactNode }) => {
  const [currentProjectID, setCurrentProjectID] = useState<number>(-1)
  return (
    <ProjectContext.Provider value={{ currentProjectID, setCurrentProjectID }}>
      {children}
    </ProjectContext.Provider>
  )
}

export const useProjectContext = () => {
  const ctx = useContext(ProjectContext)
  if (!ctx) throw new Error("useProject must be used within a ProjectProvider")
  return ctx
}
