export const themes = {
  pastel: {
    background: 'bg-[#f8f6f0]',
    card: 'bg-[#fef3c7]',
    text: 'text-gray-800',
    accent: 'bg-[#a5f3fc]',
  },
  dark: {
    background: 'bg-gray-900',
    card: 'bg-gray-800',
    text: 'text-white',
    accent: 'bg-green-400',
  },
  light: {
    background: 'bg-white',
    card: 'bg-gray-100',
    text: 'text-black',
    accent: 'bg-blue-300',
  },
} as const

export type ThemeName = keyof typeof themes
