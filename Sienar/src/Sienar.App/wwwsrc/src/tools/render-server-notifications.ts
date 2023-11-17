import { dispatchToast } from '@sienar/razor-client';

window.sienar.notifications.forEach(t => dispatchToast(t));