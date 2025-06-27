import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import TaskList from './pages/TaskList'
import { ThemeProvider } from './contexts/ThemeContext'

function App() {
  return (
    <ThemeProvider>
    <Router>
      <Routes>
        <Route path="/" element={<TaskList />} />
      </Routes>
    </Router>
    </ThemeProvider>
  )
}

export default App
