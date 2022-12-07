import { searchedModels } from "../../common/config";
import { getPostsBySearchTerm } from "../posts/postsService";
import { getUsersBySearchTerm } from "../users/usersService";
import { getBooksBySearchTerm } from "../education/booksService";

export const searchByTerm = async (searchTerm, searchModel, page, pageCount) => {
    if (searchTerm.length > 0) {
        if (searchModel.localeCompare(searchedModels.Users) === 0) {
            return {
                list: await getUsersBySearchTerm(searchTerm, page, pageCount),
                searchedModel: searchModel,
            };
        } else if (searchModel.localeCompare(searchedModels.Posts) === 0) {
            return {
                list: await getPostsBySearchTerm(searchTerm, page, pageCount),
                searchedModel: searchModel,
            };
        } else if (searchModel.localeCompare(searchedModels.Books) === 0) {
            return {
                list: await getBooksBySearchTerm(searchTerm, page, pageCount),
                searchedModel: searchModel,
            };
        }
    } else {
        return {
            list: [],
            searchedModel: searchModel,
        };
    }
};
