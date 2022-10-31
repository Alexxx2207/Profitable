import { searchedModels } from "../../common/config";
import { getPostsBySearchTerm } from "./searchPostService";
import { getUsersBySearchTerm } from "./searchUserService";

export const searchByTerm = async (searchTerm, searchModel) => {
    if (searchTerm.length > 0) {
        if (searchModel.localeCompare(searchedModels.Users) === 0) {
            return {
                list: await getUsersBySearchTerm(searchTerm),
                searchedModel: searchModel,
            };
        } else if (searchModel.localeCompare(searchedModels.Posts) === 0) {
            return {
                list: await getPostsBySearchTerm(searchTerm),
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
