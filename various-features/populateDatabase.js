const axios = require('axios')

const API_URL = 'http://localhost:5000'

const NUMBER_OF_PROJECTS = 12
const NUMBER_OF_TASKS = 100

async function populateDatabase() {
  try {
    
        console.log('1. Generating users...')
    await axios.post(`${API_URL}/users/generate`)
    console.log('Users generated\n')

    console.log(`2. Generating ${NUMBER_OF_PROJECTS} projects...`)

    await axios.post(`${API_URL}/projects/generate?numberOfProjects=${NUMBER_OF_PROJECTS}`, {
      numberOfProjects: NUMBER_OF_PROJECTS
    }).catch(err => {
      console.error('Error generating projects:', err.response?.data || err.message)
      throw err
    })
    console.log('Projects generated\n')
    

    console.log(`3. Generating ${NUMBER_OF_TASKS} tasks...`)
    await axios.post(`${API_URL}/tasks/generate?numberOfTasks=${NUMBER_OF_TASKS}`, {
      numberOfTasks: NUMBER_OF_TASKS
    })
    console.log('Tasks generated')

    console.log('Database population complete!')
  } catch (err) {
    console.error('Error populating database:', err.response?.data || err.message)
  }
}

populateDatabase()
