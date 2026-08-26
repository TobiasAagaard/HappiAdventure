import type { Activity } from '../../lib/types'

// Presentation concern, so it lives client-side rather than in the API.
const ACTIVITY_EMOJI: Record<string, string> = {
  date: '❤️',
  family_day: '👨‍👩‍👧',
  pub_crawl: '🍻',
  food_tour: '🍽️',
  team_building: '🤝',
  wine_tasting: '🍷',
  birthday_party: '🎂',
}

interface Props {
  activities: Activity[]
  selected: string | null
  onSelect: (code: string | null) => void
}

export function ActivityPicker({ activities, selected, onSelect }: Props) {
  return (
    <div className="activity-picker" role="group" aria-label="Activity filter">
      <button
        className={`chip ${selected === null ? 'chip--active' : ''}`}
        onClick={() => onSelect(null)}
      >
        ✨ Everything
      </button>
      {activities.map((activity) => (
        <button
          key={activity.id}
          className={`chip ${selected === activity.code ? 'chip--active' : ''}`}
          onClick={() => onSelect(selected === activity.code ? null : activity.code)}
        >
          {ACTIVITY_EMOJI[activity.code] ?? '📍'} {activity.name}
        </button>
      ))}
    </div>
  )
}