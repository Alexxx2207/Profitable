const requester = (method, url, data, headers = {}) => {
    let options = {
        method,
        headers
    };

    if (data) {
        options.headers['Content-Type'] = 'application/json';
        options.body = JSON.stringify(data);
    }

    return fetch(url, options);
}

export const request = {
    get: requester.bind(null, 'GET'),
    post: requester.bind(null, 'POST'),
    put: requester.bind(null, 'PUT'),
    del: requester.bind(null, 'DELETE')
}
