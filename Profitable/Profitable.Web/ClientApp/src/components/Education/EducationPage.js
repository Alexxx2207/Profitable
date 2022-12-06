import { useCallback, useEffect, useReducer } from "react";
import { BOOKS_PAGE_COUNT } from "../../common/config";
import { getBooks } from "../../services/education/booksService";
import { BookList } from "./BooksList/BooksList";
import { ShowMoreButton } from "../Common/ShowMoreButton/ShowMoreButton";
import { MessageBoxContext } from "../../contexts/MessageBoxContext";

import styles from "./EducationPage.module.css";
import { useContext } from "react";

const reducer = (state, action) => {
    switch (action.type) {
        case "loadMoreBooks":
            return {
                ...state,
                booksPage: state.booksPage + 1,
                books: [...state.books, ...action.payload],
            };
        case "changeShowMore":
            return {
                ...state,
                showShowMore: action.payload,
            };
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
        showShowMore: true
    });

    useEffect(() => {
        getBooks(0, BOOKS_PAGE_COUNT)
            .then(books => {
                if (books.length > 0) {
                    setState({
                        type: "loadMoreBooks",
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
            loadBooks(state.booksPage, BOOKS_PAGE_COUNT);
        },
        [loadBooks, state.booksPage]
    );

    return(
        <div>
            <section className={styles.booksSection}>
                <h1 className={styles.bookSectionHeader}>Books Section</h1>
                    <div className={styles.bookListContainer}>
                        <BookList books={state.books} />
                    </div>
                    <ShowMoreButton 
                        entity={"Books"}
                        showShowMore={state.showShowMore}
                        handler={handleShowMoreBooksClick} />
            </section>
        </div>
    );
}