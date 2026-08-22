import { Map } from "@vis.gl/react-maplibre"
import 'maplibre-gl/dist/maplibre-gl.css'

const middleOfEurope = [10.4515, 51.1657]

const mapStyleUrl = 'https://tiles.openfreemap.org/styles/liberty'

function App() {

  return (
    <Map 
      initialViewState={{
        longitude: middleOfEurope[0],
        latitude: middleOfEurope[1],
        zoom: 3.5
      }}
      style={{width: "100vw", height: "100vh"}}
      mapStyle={mapStyleUrl}
    />
  )
}

export default App
