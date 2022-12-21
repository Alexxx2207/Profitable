import { useCallback, useContext, useEffect, useReducer } from "react";
import { useLocation, useNavigate, Link } from "react-router-dom";
import { CLIENT_ERROR_TYPE, InstrumentTypes, LongDirectionName, POSITIONS_RECORDS_PAGE_COUNT, ShortDirectionName } from "../../../../../common/config";
import { AuthContext } from "../../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../../contexts/MessageBoxContext";
import { TimeContext } from "../../../../../contexts/TimeContext";
import { createClientErrorObject } from "../../../../../services/common/createValidationErrorObject";
import { isEmptyOrWhiteSpaceFieldChecker } from "../../../../../services/common/errorValidationCheckers";
import { createFuturesPosition } from "../../../../../services/positions/futuresPositionsService";

import { getUserPositionsRecordsByInstrumentType } from "../../../../../services/positions/positionsRecordsService";
import { createStocksPosition } from "../../../../../services/positions/stocksPositionsService";
import { convertFullDateTime } from "../../../../../utils/Formatters/timeFormatter";
import { ErrorWidget } from "../../../../Common/ErrorWidget/ErrorWidget";
import { ShowMoreButton } from "../../../../Common/ShowMoreButton/ShowMoreButton";
import { SavePositionsRecordListWidget } from "./SavePositionsRecordListWidget/SavePositionsRecordListWidget";

import styles from "./AddPositionToRecord.module.css";
import { getUserEmailFromJWT } from "../../../../../services/users/usersService";

const initialState = {
    userEmail: "",
    lists: [],
    page: 0,
    showShowMore: true,
    name: "",
    errors: {
        nameEmpty: {
            text: "Insert Name",
            fulfilled: false,
            type: CLIENT_ERROR_TYPE,
        },
    }
};

const reducer = (pageState, action) => {
    switch (action.type) {
        case "setUserEmail": {
            return {
                ...pageState,
                userEmail: action.payload,
            };
        }
        case "increasePageCount": {
            return {
                ...pageState,
                page: pageState.page + 1,
            };
        }
        case "loadRecords": {
            return {
                ...pageState,
                lists: [...pageState.lists, ...action.payload],
            };
        }
        case "loadFirstRecords": {
            return {
                ...pageState,
                lists: [...action.payload],
            };
        }
        case "hideShowMoreButton": {
            return {
                ...pageState,
                showShowMore: action.payload,
            };
        }
        case "updateInput":
            return {
                ...pageState,
                name: action.payload.value,
                errors: {
                    ...pageState.errors,
                    [action.payload.name + "Empty"]: createClientErrorObject(
                        pageState.errors[action.payload.name + "Empty"],
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.value)
                    ),
                },
            };
        default:
            return {
                ...pageState,
            };
    }
};

export const AddPositionToRecord = () => {

    const [pageState, setState] = useReducer(reducer, initialState);

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { timeOffset } = useContext(TimeContext);

    const { state } = useLocation();

    const navigate = useNavigate();

    const loadRecords = useCallback(
        (page, pageCount) => {
            getUserPositionsRecordsByInstrumentType(
                state.userEmail,
                page,
                pageCount,
                state.instrumentGroup
            ).then((result) => {
                if (result.length > 0) {
                    var recordsWithOffsetedTime = [
                        ...result.map((record) => ({
                            ...record,
                            lastUpdated: convertFullDateTime(
                                new Date(
                                    new Date(record.lastUpdated).getTime() - timeOffset * 60000
                                )
                            ),
                        })),
                    ];
                    setState({
                        type: "loadRecords",
                        payload: [...recordsWithOffsetedTime],
                    });
                } else {
                    setMessageBoxSettings("There are no more records", false);
                    setState({
                        type: "hideShowMoreButton",
                        payload: false,
                    });
                }
            });
        },
        [state.userEmail, setMessageBoxSettings, timeOffset, state.instrumentGroup]
    );

    const handleShowMoreRecordClick = useCallback(
        (e) => {
            e.preventDefault();
            setState({
                type: "increasePageCount",
            });
            loadRecords(
                pageState.page + 1,
                POSITIONS_RECORDS_PAGE_COUNT,
            );
        },
        [loadRecords, pageState.page]
    );
    
        useEffect(() => {
            getUserEmailFromJWT(JWT)
                .then(email => setState({
                    type: "setUserEmail",
                    payload: email,
                }));
        }, [JWT]);

    useEffect(() => {
        getUserPositionsRecordsByInstrumentType(
            state.userEmail,
            0,
            POSITIONS_RECORDS_PAGE_COUNT,
            state.instrumentGroup
        ).then((result) => {
            if (result.length > 0) {
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
            } else {
                setState({
                    type: "hideShowMoreButton",
                    payload: false,
                });
            }
        });
    }, [state.userEmail, timeOffset, state.instrumentGroup]);

    const handleSave = (record) => {
        const instrumentGroup = record.instrumentGroup;
        if(instrumentGroup === InstrumentTypes.futures) {
            createFuturesPosition(
                JWT,
                record.guid,
                state.position.name,
                state.position.directionBullish ? LongDirectionName : ShortDirectionName,
                state.position.entryPrice,
                state.position.exitPrice,
                state.position.numberOfContracts,
                state.position.tickSize,
                state.position.tickValue
            )
                .then((jwt) => {
                    setMessageBoxSettings("The position was created successfully!", true);
                    navigate(
                        `/users/${state.userEmail}/positions-records/futures/${record.guid}`
                    );
                })
        } else if(instrumentGroup === InstrumentTypes.stocks) {
            const clientErrors = Object.values(pageState.errors).filter(
                (err) => err.type === CLIENT_ERROR_TYPE
            );
    
            if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
                createStocksPosition(
                    JWT,
                    record.guid,
                    pageState.name,
                    state.position.entryPrice,
                    state.position.exitPrice,
                    state.position.numberOfShares,
                    state.position.buyCommission,
                    state.position.sellCommission
                )
                .then(() => {
                    setMessageBoxSettings("The position was created successfully!", true);
                    navigate(
                        `/users/${state.userEmail}/positions-records/stocks/${record.guid}`
                    );
                })
            }
        }
    };

    const changeHandler = (e) => {
        setState({
            type: "updateInput",
            payload: e.target,
        });
    };

    return (
        <div className={styles.pageContainer}>
            {state.instrumentGroup === InstrumentTypes.stocks ?
                <div className={styles.stockNameForm}>
                   <div className={styles.formGroup}>
                            <div>
                                <h5>Position Name</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                type="text"
                                name="name"
                                placeholder={"AAPL/GOOG/NVDA..."}
                                value={state.name}
                                onChange={changeHandler}
                            />
                        </div>
                <aside className={styles.createAside}>
                    <div className={styles.errorsContainer}>
                        {Object.values(pageState.errors).map((error, index) => (
                            <ErrorWidget key={index} error={error} />
                        ))}
                    </div>
                </aside>
                </div>
                :
                <></>
            }
            <div className={styles.recordsListHeader}>
                <h1>Records List</h1>
            </div>
            <div className={styles.listContainer}>
                {pageState.lists.length > 0 ? (
                    pageState.lists.map((list) => (
                        <SavePositionsRecordListWidget
                            key={list.guid}
                            list={list}
                            handleSave={handleSave}
                        />
                    ))
                ) : (
                    <h2 className={styles.noRecordsHeader}>
                        No Records Made Yet - Go To <Link 
                        to={`/users/${pageState.userEmail}/account-statistics`}>
                            Records
                        </Link> to create one</h2>
                )}
            </div>
            
            <ShowMoreButton 
                entity={"Records"}
                showShowMore={pageState.showShowMore}
                handler={handleShowMoreRecordClick} />
    </div>
        
    );
}