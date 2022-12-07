import { useCallback, useEffect, useReducer } from "react";
import { BOOKS_PAGE_COUNT, searchedModels, SEARCH_ENTITY_IN_PAGE_COUNT } from "../../common/config";
import { getBooks } from "../../services/education/booksService";
import { BookList } from "./BooksList/BooksList";
import { ShowMoreButton } from "../Common/ShowMoreButton/ShowMoreButton";
import { GoToTop } from "../Common/GoToTop/GoToTop";
import { MessageBoxContext } from "../../contexts/MessageBoxContext";
import styles from "./EducationPage.module.css";
import { useContext } from "react";
import { searchByTerm } from "../../services/search/searchService";

import debounce from "lodash.debounce";


const reducer = (state, action) => {
    switch (action.type) {
        case "loadMoreBooks":
            return {
                ...state,
                booksPage: state.booksPage + 1,
                books: [...state.books, ...action.payload],
            };
        case "loadFirstBooks":
            return {
                ...state,
                booksPage: state.booksPage + 1,
                books: [...action.payload],
            };
        case "loadSearchedBooks":
            return {
                ...state,
                booksPage: 0,
                books: [...action.payload],
            };
        case "changeShowMore":
            return {
                ...state,
                showShowMore: action.payload,
            };
        case "changeSearchTerm":
            return {
                ...state,
                bookSearchTerm: action.payload,
                showShowMore: true,
            };
        case "setBookPages": 
        return {
            ...state,
            booksPage: action.payload
        }
        default:
            return {
                ...state
            };
    }
}

export const EducationPage = () => {

    var { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(reducer, {
        books: [],
        booksPage: 0,
        showShowMore: true,
        bookSearchTerm: ""
    });

    const loadFirstBooks = useCallback(() => {
         getBooks(0, BOOKS_PAGE_COUNT)
            .then(books => {
                if (books.length > 0) {
                    setState({
                        type: "loadFirstBooks",
                        payload: books
                    });
                } else {
                    setState({
                        type: "changeShowMore",
                        payload: false
                    });
                }
            });
    }, []);

    useEffect(() => {
        loadFirstBooks();
    }, []);

    const loadBooks = useCallback(
        (page, pageCount) => {
            getBooks(page, pageCount).then((books) => {
                if (books.length > 0) {
                    setState({
                        type: "loadMoreBooks",
                        payload: books
                    });
                } else {
                    setMessageBoxSettings("There are no more books", false);
                    setState({
                        type: "changeShowMore",
                        payload: false
                    });
                }
            });
        },
        []
    );

    const handleShowMoreBooksClick = useCallback
    (
        (e) => {
            e.preventDefault();
            setState({
                type: ""
            });
            if(state.bookSearchTerm) {
                searchByTerm(state.bookSearchTerm, searchedModels.Books, state.booksPage + 1, SEARCH_ENTITY_IN_PAGE_COUNT).then(
                    (result) => {
                        setState({
                            type: "loadMoreBooks",
                            payload: [...result.list],
                        });
                        
                        console.log(result.list.length);
                        if (result.list.length <= 0) {
                            setState({
                                type: "changeShowMore",
                                payload: false,
                            });
                        }
                    }
                );
            } else {
                loadBooks(state.booksPage, BOOKS_PAGE_COUNT);
            }
        },
        [loadBooks, state.booksPage]
    );

    const searchTermOnChange = (e) => {
        setState({
            type: "changeSearchTerm",
            payload: e.target.value,
        });

        if(!e.target.value) {
            loadFirstBooks();
            setState({
                type: "setBookPages",
                payload: 0,
            });

            return;
        }

        searchByTerm(e.target.value, searchedModels.Books, 0, SEARCH_ENTITY_IN_PAGE_COUNT).then(
            (result) => {
                setState({
                    type: "loadSearchedBooks",
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

    return(
        <main>
            <section className={styles.booksSection}>
                <h1 className={styles.bookSectionHeader}>Books Section</h1>
                <div className={styles.searchInputContainer}>
                    <input
                        placeholder={"Search book..."}
                        type="text"
                        onChange={optimisedOnChange}
                        className={styles.searchInput}
                    />
                </div>
                    <div className={styles.bookListContainer}>
                        <BookList books={state.books} />
                    </div>
                    <ShowMoreButton 
                        entity={"Books"}
                        showShowMore={state.showShowMore}
                        handler={handleShowMoreBooksClick} />
            </section>

            <GoToTop />
        </main>
    );
}