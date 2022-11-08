import { searchedModels } from "../../common/config";
import { getPostsBySearchTerm } from "./searchPostService";
import { getUsersBySearchTerm } from "./searchUserService";

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
        }
    } else {
        return {
            list: [],
            searchedModel: searchModel,
        };
    }
};
