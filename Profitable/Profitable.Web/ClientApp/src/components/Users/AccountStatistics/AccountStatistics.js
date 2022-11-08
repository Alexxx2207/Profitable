import { useLocation, useNavigate, useParams } from "react-router-dom";
import { useCallback, useContext, useEffect, useReducer, useState } from "react";

import {
    getPositionsRecordsOrderByOptions,
    getUserPositionsRecords,
} from "../../../services/positions/positionsRecordsService";
import { convertFullDateTime } from "../../../utils/Formatters/timeFormatter";
import { PositionsRecordListsList } from "./PositionsRecords/PositionsRecordListsList/PositionsRecordListsList";
import {
    POSITIONS_RECORDS_DEFAULT_ORDER,
    POSITIONS_RECORDS_PAGE_COUNT,
} from "../../../common/config";
import { getUserEmailFromJWT } from "../../../services/users/usersService";
import { AuthContext } from "../../../contexts/AuthContext";
import { TimeContext } from "../../../contexts/TimeContext";

import styles from "./AccountStatistics.module.css";

const initialState = {
    lists: [],
    page: 1,
    orderPositionsRecordsBySelected: POSITIONS_RECORDS_DEFAULT_ORDER,
    positionsRecordsOrderByOptions: [],
};

const reducer = (state, action) => {
    switch (action.type) {
        case "increasePageCount": {
            return {
                ...state,
                page: state.page + 1,
            };
        }
        case "loadOrderByOptions": {
            return {
                ...state,
                positionsRecordsOrderByOptions: action.payload,
            };
        }
        case "loadRecords": {
            return {
                ...state,
                lists: [...state.lists, ...action.payload],
            };
        }
        case "loadFirstRecords": {
            return {
                ...state,
                lists: [...action.payload],
            };
        }
        case "changeSelectedOrderOption": {
            return {
                ...state,
                orderPositionsRecordsBySelected: action.payload,
            };
        }
        default:
            return {
                ...state,
            };
    }
};

export const AccountStatistics = () => {
    const { JWT, removeAuth } = useContext(AuthContext);
    const { timeOffset } = useContext(TimeContext);

    const navigate = useNavigate();

    const location = useLocation();

    const { searchedProfileEmail } = useParams();

    const [state, setState] = useReducer(reducer, initialState);

    const [loggedInUserEmail, setLoggedInUserEmail] = useState({});

    const handleAddRecordButtonClick = () => {
        navigate(`/users/${searchedProfileEmail}/positions-records/create`);
    };

    const loadRecords = useCallback(
        (page, pageCount, orderPositionsRecordsBySelected) => {
            getUserPositionsRecords(
                searchedProfileEmail,
                page,
                pageCount,
                orderPositionsRecordsBySelected
            ).then((result) => {
                var recordsWithOffsetedTime = [
                    ...result.map((record) => ({
                        ...record,
                        lastUpdated: convertFullDateTime(
                            new Date(new Date(record.lastUpdated).getTime() - timeOffset * 60000)
                        ),
                    })),
                ];
                setState({
                    type: "loadRecords",
                    payload: [...recordsWithOffsetedTime],
                });
            });
        },
        [searchedProfileEmail]
    );

    const handleScroll = useCallback(
        (e) => {
            const scrollHeight = e.target.documentElement.scrollHeight;
            const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

            if (currentHeight >= scrollHeight - 1 || currentHeight === scrollHeight) {
                setState({
                    type: "increasePageCount",
                });
                loadRecords(
                    state.page,
                    POSITIONS_RECORDS_PAGE_COUNT,
                    state.orderPositionsRecordsBySelected
                );
            }
        },
        [loadRecords, state.page, state.orderPositionsRecordsBySelected]
    );

    useEffect(() => {
        window.addEventListener("scroll", handleScroll);

        return () => {
            window.removeEventListener("scroll", handleScroll);
        };
    }, [handleScroll]);

    useEffect(() => {
        getPositionsRecordsOrderByOptions().then((result) => {
            setState({
                type: "loadOrderByOptions",
                payload: result.types,
            });
        });

        getUserEmailFromJWT(JWT)
            .then((result) => setLoggedInUserEmail((email) => result))
            .catch((err) => {
                removeAuth();
                navigate(location.pathname);
            });
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        getUserPositionsRecords(
            searchedProfileEmail,
            0,
            POSITIONS_RECORDS_PAGE_COUNT,
            POSITIONS_RECORDS_DEFAULT_ORDER
        ).then((result) => {
            var recordsWithOffsetedTime = [
                ...result.map((record) => ({
                    ...record,
                    lastUpdated: convertFullDateTime(
                        new Date(new Date(record.lastUpdated).getTime() - timeOffset * 60000)
                    ),
                })),
            ];
            setState({
                type: "loadFirstRecords",
                payload: [...recordsWithOffsetedTime],
            });
        });
    }, [searchedProfileEmail]);

    const changeSelectedOrderOption = (e) => {
        setState({
            type: "changeSelectedOrderOption",
            payload: e.target.value,
        });

        getUserPositionsRecords(
            searchedProfileEmail,
            0,
            POSITIONS_RECORDS_PAGE_COUNT,
            e.target.value
        ).then((result) => {
            var recordsWithOffsetedTime = [
                ...result.map((record) => ({
                    ...record,
                    lastUpdated: convertFullDateTime(
                        new Date(new Date(record.lastUpdated).getTime() - timeOffset * 60000)
                    ),
                })),
            ];
            setState({
                type: "loadFirstRecords",
                payload: [...recordsWithOffsetedTime],
            });
        });
    };

    return (
        <div>
            <div className={styles.recordsListHeader}>
                <h1>Records List</h1>

                {searchedProfileEmail.localeCompare(loggedInUserEmail) === 0 ? (
                    <button onClick={handleAddRecordButtonClick} className={styles.addRecordButton}>
                        + Add Record
                    </button>
                ) : (
                    ""
                )}
            </div>
            <div className={styles.orderSelectorContainer}>
                <h5 className={styles.orderByHeader}>Order By</h5>
                <select
                    onChange={changeSelectedOrderOption}
                    value={state.orderPositionsRecordsBySelected}
                    className={styles.orderBySelector}
                >
                    {state.positionsRecordsOrderByOptions.map((option, index) => (
                        <option key={index} value={option.split(" ").join("")}>
                            {option}
                        </option>
                    ))}
                </select>
            </div>
            <PositionsRecordListsList
                records={state.lists}
                showOwnerActionButtons={searchedProfileEmail.localeCompare(loggedInUserEmail) === 0}
            />
        </div>
    );
};
