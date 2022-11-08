import { useReducer } from "react";
import { searchedModels } from "../../common/config";
import { searchByTerm } from "../../services/search/searchService";

import { UserSearchResult } from "./UserSearchResult/UserSearchResult";
import { PostsList } from "../PostsAndComments/Posts/PostsList/PostsList";

import debounce from "lodash.debounce";

import styles from "./SearchPage.module.css";

const intialState = {
    searchedTerm: "",
    searchedModel: Object.getOwnPropertyNames(searchedModels)[0],
    searchedModels: Object.getOwnPropertyNames(searchedModels),
    searchResults: [],
};

const reducer = (state, action) => {
    switch (action.type) {
        case "changeSearchResults":
            return {
                ...state,
                searchResults: action.payload.newResults,
                searchedModel: action.payload.newModel,
            };
        case "changeSearchTerm":
            return {
                ...state,
                searchedTerm: action.payload,
            };
        default:
            return state;
    }
};

export const SearchPage = () => {
    const [state, setState] = useReducer(reducer, intialState);

    const searchTermOnChange = (e) => {
        setState({
            type: "changeSearchTerm",
            payload: e.target.value,
        });
        searchByTerm(e.target.value, state.searchedModel).then((result) => {
            setState({
                type: "changeSearchResults",
                payload: {
                    newResults: result.list,
                    newModel: result.searchedModel,
                },
            });
        });
    };

    const optimisedOnChange = debounce(searchTermOnChange, 700);

    const searchModelOnChange = (e) => {
        searchByTerm(state.searchedTerm, e.target.value).then((result) => {
            setState({
                type: "changeSearchResults",
                payload: {
                    newResults: result.list,
                    newModel: result.searchedModel,
                },
            });
        });
    };

    const assessWhatModelToDisplay = () => {
        if (state.searchedModel.localeCompare(searchedModels.Users) === 0) {
            return (
                <div className={styles.userResultsContainer}>
                    {state.searchResults.map((resultItem) => (
                        <UserSearchResult user={resultItem} />
                    ))}
                </div>
            );
        } else if (state.searchedModel.localeCompare(searchedModels.Posts) === 0) {
            return <PostsList posts={state.searchResults} />;
        }
    };

    return (
        <div className={styles.searchPageContainer}>
            <h1 className={styles.searchPageHeader}>Search Whatever You Need</h1>
            <div className={styles.searchInputContainer}>
                <input
                    placeholder={"Search here..."}
                    type="text"
                    onChange={optimisedOnChange}
                    className={styles.searchInput}
                />
            </div>
            <div className={styles.searchForSelectorContainer}>
                <h4 className={styles.searchForSelectorHeader}>Search For: </h4>
                <select
                    onChange={searchModelOnChange}
                    value={state.searchedModel}
                    className={styles.searchModelSelector}
                >
                    {state.searchedModels.map((model, index) => (
                        <option key={index} value={model}>
                            {model}
                        </option>
                    ))}
                </select>
            </div>
            <section className={styles.resultSection}>
                {state.searchResults.length > 0 ? (
                    <>
                        <h2 className={styles.searchedResultsHeader}>Searched Results</h2>
                        <div className={styles.searchedResultsContainer}>
                            {assessWhatModelToDisplay()}
                        </div>
                    </>
                ) : state.searchedTerm.length > 0 ? (
                    <h2 className={styles.noMatchingHeader}>No Matching {state.searchedModel}</h2>
                ) : (
                    ""
                )}
            </section>
        </div>
    );
};
