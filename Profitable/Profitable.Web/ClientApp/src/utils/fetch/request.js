const requester = (method, url, data) => {
    let options = {
        method
    };

    if (data) {
        options.headers = {};

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
