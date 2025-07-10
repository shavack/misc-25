export const themes = {
  pastel: {
    background: 'bg-[#f8fafc]',
    card: 'bg-[#fef6fb]',
    text: 'text-[#5b5b5b]',
    accent: 'bg-[#b5ead7]',
    overdue: 'bg-[#ffd6e0]',
    inProgressBackground: 'bg-[#fff9c4]',
    inProgressText: 'text-[#b59f3b]',
    completedBackground: 'bg-[#c8e6c9]',
    completedText: 'text-[#388e3c]',
    notStartedBackground: 'bg-[#e0e0e0]',
    notStartedText:'text-[#616161]',
    tag: 'bg-[#d1c4e9] text-[#5e35b1]'
  },
  dark: {
    background: 'bg-[#18181b]',
    card: 'bg-[#27272a]',
    text: 'text-[#f4f4f5]',
    accent: 'bg-[#60a5fa]',
    overdue: 'bg-[#f87171]',
    inProgressBackground: 'bg-[#facc15]',
    inProgressText: 'text-[#78350f]',
    completedBackground: 'bg-[#22c55e]',
    completedText: 'text-[#052e16]',
    notStartedBackground: 'bg-[#52525b]',
    notStartedText:'text-[#d4d4d8]',
    tag: 'bg-[#818cf8] text-[#1e3a8a]'
  },
  light: {
    background: 'bg-[#f9fafb]',
    card: 'bg-[#f3f4f6]',
    text: 'text-[#18181b]',
    accent: 'bg-[#38bdf8]',
    overdue: 'bg-[#f87171]',
    inProgressBackground: 'bg-[#fde68a]',
    inProgressText: 'text-[#92400e]',
    completedBackground: 'bg-[#bbf7d0]',
    completedText: 'text-[#166534]',
    notStartedBackground: 'bg-[#e5e7eb]',
    notStartedText:'text-[#374151]',
    tag: 'bg-[#c7d2fe] text-[#1e3a8a]'
  },
} as const

export type ThemeName = keyof typeof themes
