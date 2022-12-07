import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getBooks = async (page, pageCount) => {
    var response = await request.get(`${WEB_API_BASE_URL}/education/books?page=${page}&pageCount=${pageCount}`);

    return response.json();
}

export const getBooksBySearchTerm = (searchTerm, page, pageCount) => {
    return request
        .get(`${WEB_API_BASE_URL}/search/books/${searchTerm}?page=${page}&pageCount=${pageCount}`)
        .then((response) => response.json());
}