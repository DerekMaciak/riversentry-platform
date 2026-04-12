// Device Map JavaScript Module using Leaflet
// Stores map instances by element ID
const maps = {};

// Status colors for device markers
const statusColors = {
    Armed: '#3fb950',       // Green
    AlarmWater: '#f85149',  // Red
    AlarmUpstream: '#ff6b6b', // Light red
    AlarmSilent: '#d29922', // Yellow/Orange
    AlarmDrill: '#4A90D9',  // Blue
    Offline: '#6e7681',     // Gray
    Unknown: '#8b949e'      // Light gray
};

export function initializeMap(mapId, centerLat, centerLng, zoom, markers) {
    // Clean up existing map if any
    if (maps[mapId]) {
        maps[mapId].remove();
        delete maps[mapId];
    }

    // Create map
    const map = L.map(mapId, {
        zoomControl: true,
        attributionControl: true
    }).setView([centerLat, centerLng], zoom);

    // Add tile layer (OpenStreetMap + Satellite hybrid)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        maxZoom: 19
    }).addTo(map);

    // Store map instance
    maps[mapId] = { map, markers: [] };

    // Add markers
    addMarkers(mapId, markers);

    // Fit bounds to show all markers
    if (markers && markers.length > 0) {
        fitBoundsToMarkers(mapId);
    }
}

function addMarkers(mapId, markers) {
    const instance = maps[mapId];
    if (!instance) return;

    // Clear existing markers
    instance.markers.forEach(m => instance.map.removeLayer(m));
    instance.markers = [];

    // Add new markers
    markers.forEach(device => {
        const color = device.isAlarming ? '#f85149' : 
                      !device.isOnline ? '#6e7681' : 
                      (statusColors[device.state] || '#3fb950');

        // Create custom icon with status color
        const icon = L.divIcon({
            className: 'device-marker',
            html: `<div class="marker-pin" style="background-color: ${color}">
                     <span class="marker-icon">${device.isAlarming ? '⚠️' : '📡'}</span>
                   </div>
                   <div class="marker-pulse" style="background-color: ${color}"></div>`,
            iconSize: [30, 42],
            iconAnchor: [15, 42],
            popupAnchor: [0, -42]
        });

        const marker = L.marker([device.lat, device.lng], { icon })
            .addTo(instance.map)
            .bindPopup(createPopupContent(device));

        // Add pulsing animation for alarming devices
        if (device.isAlarming) {
            marker.getElement()?.classList.add('alarming');
        }

        instance.markers.push(marker);
    });
}

function createPopupContent(device) {
    const statusClass = device.isAlarming ? 'status-alarm' : 
                        device.isOnline ? 'status-online' : 'status-offline';
    const statusText = device.isAlarming ? '⚠️ ALARM' : 
                       device.isOnline ? '✅ Online' : '❌ Offline';

    return `
        <div class="device-popup">
            <h4>${device.name}</h4>
            <p class="device-family">${device.familyName}</p>
            <p class="device-status ${statusClass}">${statusText}</p>
            <p class="device-state">State: ${device.state}</p>
            <p class="device-coords">
                <small>${device.lat.toFixed(6)}, ${device.lng.toFixed(6)}</small>
            </p>
        </div>
    `;
}

export function updateMarkers(mapId, markers) {
    addMarkers(mapId, markers);
}

export function fitBoundsToMarkers(mapId) {
    const instance = maps[mapId];
    if (!instance || instance.markers.length === 0) return;

    const group = L.featureGroup(instance.markers);
    instance.map.fitBounds(group.getBounds().pad(0.1));
}

export function centerOnDevice(mapId, lat, lng, zoom = 16) {
    const instance = maps[mapId];
    if (!instance) return;
    instance.map.setView([lat, lng], zoom);
}

export function destroyMap(mapId) {
    if (maps[mapId]) {
        maps[mapId].map.remove();
        delete maps[mapId];
    }
}
