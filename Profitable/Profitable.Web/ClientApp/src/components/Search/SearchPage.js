import { useCallback, useContext, useReducer } from "react";
import { searchedModels, SEARCH_ENTITY_IN_PAGE_COUNT } from "../../common/config";
import { searchByTerm } from "../../services/search/searchService";

import { UserSearchResult } from "./UserSearchResult/UserSearchResult";
import { PostsList } from "../PostsAndComments/Posts/PostsList/PostsList";

import debounce from "lodash.debounce";

import styles from "./SearchPage.module.css";
import { MessageBoxContext } from "../../contexts/MessageBoxContext";

const intialState = {
    searchedTerm: "",
    searchedModel: Object.getOwnPropertyNames(searchedModels)[0],
    searchedModels: Object.getOwnPropertyNames(searchedModels),
    searchResults: [],
    page: 0,
    showShowMore: true,
};

const reducer = (state, action) => {
    switch (action.type) {
        case "loadEntities":
            return {
                ...state,
                page: state.page + 1,
                searchResults: [...state.searchResults, ...action.payload],
            };
        case "changeSearchResults":
            return {
                ...state,
                searchResults: [...action.payload],
            };
        case "changeSearchModel":
            return {
                ...state,
                page: 0,
                showShowMore: true,
                searchResults: action.payload.list,
                searchedModel: action.payload.newModel,
            };
        case "changeSearchTerm":
            return {
                ...state,
                page: 0,
                showShowMore: true,
                searchedTerm: action.payload,
            };
        case "hideShowMoreButton":
            return {
                ...state,
                showShowMore: action.payload,
            };
        default:
            return state;
    }
};

export const SearchPage = () => {
    const [state, setState] = useReducer(reducer, intialState);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const searchTermOnChange = (e) => {
        setState({
            type: "changeSearchTerm",
            payload: e.target.value,
        });
        searchByTerm(e.target.value, state.searchedModel, 0, SEARCH_ENTITY_IN_PAGE_COUNT).then(
            (result) => {
                setState({
                    type: "changeSearchResults",
                    payload: [...result.list],
                });

                if (result.list.length <= 0) {
                    setState({
                        type: "hideShowMoreButton",
                        payload: false,
                    });
                }
            }
        );
    };

    const optimisedOnChange = debounce(searchTermOnChange, 700);

    const searchModelOnChange = (e) => {
        searchByTerm(state.searchedTerm, e.target.value, 0, SEARCH_ENTITY_IN_PAGE_COUNT).then(
            (result) => {
                setState({
                    type: "changeSearchModel",
                    payload: {
                        list: [...result.list],
                        newModel: result.searchedModel,
                    },
                });
                if (result.list.length <= 0) {
                    setState({
                        type: "hideShowMoreButton",
                        payload: false,
                    });
                }
            }
        );
    };

    const assessWhatModelToDisplay = () => {
        if (state.searchedModel.localeCompare(searchedModels.Users) === 0) {
            return (
                <div className={styles.userResultsContainer}>
                    {state.searchResults.map((resultItem, index) => (
                        <UserSearchResult key={index} user={resultItem} />
                    ))}
                </div>
            );
        } else if (state.searchedModel.localeCompare(searchedModels.Posts) === 0) {
            return <PostsList posts={state.searchResults} />;
        }
    };

    const searchOnShowMoreClick = useCallback((page, pageCount) => {
        searchByTerm(state.searchedTerm, state.searchedModel, page, pageCount).then((result) => {
            if (result.list.length > 0) {
                setState({
                    type: "loadEntities",
                    payload: [...result.list],
                });
            } else {
                setMessageBoxSettings(
                    `There are no more ${state.searchedModel.toLowerCase()}`,
                    false
                );
                setState({
                    type: "hideShowMoreButton",
                    payload: false,
                });
            }
        });
    });

    const handleShowMoreClick = useCallback(
        (e) => {
            e.preventDefault();
            searchOnShowMoreClick(state.page + 1, SEARCH_ENTITY_IN_PAGE_COUNT);
        },
        [searchOnShowMoreClick, state.page]
    );

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

            {state.showShowMore && state.searchedTerm ? (
                <div className={styles.loadMoreContainer}>
                    <h4 className={styles.loadMoreButton} onClick={handleShowMoreClick}>
                        Show More {state.searchedModel}
                    </h4>
                </div>
            ) : (
                <></>
            )}
        </div>
    );
};
