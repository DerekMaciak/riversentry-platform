// River Sentry Service Worker for caching
const CACHE_NAME = 'riversentry-v1';

// Assets to cache immediately on install
const PRECACHE_ASSETS = [
    '/',
    '/devices',
    '/locations',
    '/alarms',
    '/_content/RiverSentry.UI.Shared/css/riversentry-shared.css',
    '/_content/RiverSentry.UI.Shared/images/lighteningbackground.png',
    '/_content/RiverSentry.UI.Shared/images/shield.png',
    '/app.css',
    '/images/logotransparent.png',
    '/images/device.png'
];

// Install - precache static assets
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => cache.addAll(PRECACHE_ASSETS))
            .then(() => self.skipWaiting())
    );
});

// Activate - clean up old caches
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames
                    .filter(name => name !== CACHE_NAME)
                    .map(name => caches.delete(name))
            );
        }).then(() => self.clients.claim())
    );
});

// Fetch - network first, fallback to cache
self.addEventListener('fetch', event => {
    // Skip non-GET requests
    if (event.request.method !== 'GET') return;

    // Skip API calls - always go to network
    if (event.request.url.includes('/api/')) return;

    // Skip SignalR
    if (event.request.url.includes('/_blazor')) return;

    event.respondWith(
        fetch(event.request)
            .then(response => {
                // Clone response to cache
                if (response.ok) {
                    const responseClone = response.clone();
                    caches.open(CACHE_NAME).then(cache => {
                        cache.put(event.request, responseClone);
                    });
                }
                return response;
            })
            .catch(() => {
                // Network failed, try cache
                return caches.match(event.request);
            })
    );
});
