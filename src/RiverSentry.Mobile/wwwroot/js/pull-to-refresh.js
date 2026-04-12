window.initPullToRefresh = function (element, dotNetRef) {
    let startY = 0;
    let pulling = false;
    let indicator = element.querySelector('.ptr-indicator');

    if (!indicator) {
        indicator = document.createElement('div');
        indicator.className = 'ptr-indicator';
        indicator.innerHTML = '<div class="ptr-spinner"></div><span>Release to refresh</span>';
        element.prepend(indicator);
    }

    element.addEventListener('touchstart', function (e) {
        if (element.scrollTop === 0) {
            startY = e.touches[0].clientY;
            pulling = true;
        }
    }, { passive: true });

    element.addEventListener('touchmove', function (e) {
        if (!pulling) return;
        const y = e.touches[0].clientY;
        const diff = y - startY;

        if (diff > 0 && diff < 150) {
            indicator.style.height = Math.min(diff * 0.6, 60) + 'px';
            indicator.style.opacity = Math.min(diff / 80, 1);
            indicator.classList.toggle('ready', diff > 80);
        }
    }, { passive: true });

    element.addEventListener('touchend', function () {
        if (!pulling) return;
        pulling = false;

        if (indicator.classList.contains('ready')) {
            indicator.style.height = '50px';
            indicator.style.opacity = '1';
            indicator.classList.add('refreshing');
            indicator.querySelector('span').textContent = 'Refreshing...';

            dotNetRef.invokeMethodAsync('OnPullRefresh').then(function () {
                setTimeout(function () {
                    indicator.style.height = '0';
                    indicator.style.opacity = '0';
                    indicator.classList.remove('ready', 'refreshing');
                    indicator.querySelector('span').textContent = 'Release to refresh';
                }, 300);
            });
        } else {
            indicator.style.height = '0';
            indicator.style.opacity = '0';
            indicator.classList.remove('ready');
        }
    });
};
