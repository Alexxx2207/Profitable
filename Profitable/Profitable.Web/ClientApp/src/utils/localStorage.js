
export function getFromLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key));
}

export function setLocalStorage(key, data) {
    localStorage.setItem(key, JSON.stringify(data));
}

export function clearLocalStorage(key) {
    localStorage.removeItem(key);
}