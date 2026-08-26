import { useEffect } from 'react'
import { Map, Marker, Popup, useMap } from '@vis.gl/react-maplibre'

import 'maplibre-gl/dist/maplibre-gl.css'
import { useMapStyle } from '../hooks/useMapStyle'
import type { Place } from '../../lib/types'


const AALBORG: [number, number] = [9.9217, 57.0488]
const MAP_STYLE_URL = 'https://tiles.openfreemap.org/styles/liberty'

// Matches the PriceLevel enum on the API.
const PRICE_LABEL = ['Free', '$', '$$', '$$$']

interface Props {
  places: Place[]
  selectedPlace: Place | null
  onSelectPlace: (place: Place | null) => void
}

/** Glides the camera to fit the current result set whenever it changes. */
function FitToPlaces({ places }: { places: Place[] }) {
  const { current: map } = useMap()

  useEffect(() => {
    if (!map || places.length === 0) return
    const lons = places.map((p) => p.location.longitude)
    const lats = places.map((p) => p.location.latitude)
    map.fitBounds(
      [[Math.min(...lons), Math.min(...lats)], [Math.max(...lons), Math.max(...lats)]],
      { padding: { top: 90, bottom: 60, left: 340, right: 60 }, maxZoom: 15, duration: 800 },
    )
  }, [map, places])

  return null
}

export function PlaceMap({ places, selectedPlace, onSelectPlace }: Props) {
  const mapStyle = useMapStyle(MAP_STYLE_URL)

  if (!mapStyle) return <div className="map-placeholder" />

  return (
    <Map
      initialViewState={{ longitude: AALBORG[0], latitude: AALBORG[1], zoom: 13 }}
      style={{ width: '100vw', height: '100vh' }}
      mapStyle={mapStyle}
      onClick={() => onSelectPlace(null)}
    >
      <FitToPlaces places={places} />

      {places.map((place) => {
        return (
          <Marker
            key={place.id}
            longitude={place.location.longitude}
            latitude={place.location.latitude}
            onClick={(e) => {
              e.originalEvent.stopPropagation()
              onSelectPlace(place)
            }}
          >
            <div
              className={`place-marker ${selectedPlace?.id === place.id ? 'place-marker--active' : ''}`}
              title={place.name}
            />
          </Marker>
        )
      })}

      {selectedPlace && (
        <Popup
          longitude={selectedPlace.location.longitude}
          latitude={selectedPlace.location.latitude}
          anchor="bottom"
          offset={16}
          closeButton={false}
          closeOnClick={false}
          onClose={() => onSelectPlace(null)}
        >
          <div className="place-popup">
            <strong>{selectedPlace.name}</strong>
            <span className="place-popup__price">
              {PRICE_LABEL[selectedPlace.priceLevel] ?? ''}
            </span>
          </div>
        </Popup>
      )}
    </Map>
  )
}
