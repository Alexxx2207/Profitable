import { useEffect, useReducer } from "react";
import classnames from "classnames";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import TextField from "@mui/material/TextField";
import { styled } from "@mui/material/styles";

import { getReport, getReportedInstruments } from "../../../services/cot/cotReportService";
import dayjs from "dayjs";
import { getLastDayOFWeekDate } from "../../../utils/date-utilities/dateFunctions";

import { plusInFrontOfPositiveNumber } from "../../../utils/Formatters/stringFormatter";

import styles from "./COTReportsPage.module.css";

const popperSx = {
    "& .PrivatePickersSlideTransition-root": {
        minHeight: "200px",
    },
    "& .MuiCalendarPicker-root, & .MuiCalendarPicker-root *": {
        backgroundColor: "white",
        color: "black",
    },
    "& .PrivatePickersYear-yearButton.Mui-selected": {
        color: "white",
    },
    "& .PrivatePickersMonth-root.Mui-selected": {
        color: "white",
    },
    "& .MuiPickersDay-dayWithMargin.Mui-selected": {
        color: "white",
    },
    "& .MuiTouchRipple-root": {
        background: "transparent",
    },
};

const CssTextField = styled(TextField)({
    "& .MuiFormHelperText-root": {
        color: "white",
        letterSpacing: "1px",
    },
    "& .MuiInputBase-formControl": {
        color: "white",
        background: "transparent",
    },
    "& .MuiOutlinedInput-notchedOutline": {
        color: "white",
        background: "transparent",
    },
    "& .MuiTouchRipple-root": {
        background: "transparent",
    },
    "& .MuiSvgIcon-root": {
        color: "white",
    },
    "& label": {
        color: "white",
        fontSize: "1.3rem",
        letterSpacing: "1px",
    },
    "& label.Mui-focused": {
        color: "white",
    },
    "& .MuiInput-underline:after": {
        borderBottomColor: "white",
    },
    "& .MuiOutlinedInput-root": {
        "& fieldset": {
            color: "white",
            borderColor: "white",
        },
        "&:hover fieldset": {
            color: "white",
            borderColor: "white",
        },
        "&.Mui-focused fieldset": {
            color: "white",
            borderColor: "white",
        },
    },
});

const reducer = (state, action) => {
    switch (action.type) {
        case "loadReportedInstruments":
            return {
                ...state,
                reportedInstruments: [...action.payload],
            };
        case "changeReport":
            return {
                ...state,
                report: {
                    instrumentGuid: action.payload.guid,
                    instrumentName: action.payload.instrumentName,
                },
            };
        case "loadReport":
            return {
                ...state,
                report: {
                    ...state.report,
                    datePublished: action.payload.datePublished,
                    assetManagersLong: action.payload.assetManagersLong,
                    assetManagersShort: action.payload.assetManagersShort,
                    leveragedFundsLong: action.payload.leveragedFundsLong,
                    leveragedFundsShort: action.payload.leveragedFundsShort,
                    assetManagersLongChange: action.payload.assetManagersLongChange,
                    assetManagersShortChange: action.payload.assetManagersShortChange,
                    leveragedFundsLongChange: action.payload.leveragedFundsLongChange,
                    leveragedFundsShortChange: action.payload.leveragedFundsShortChange,
                },
                fromDate: action.payload.datePublished,
            };
        case "setFromDate":
            return {
                ...state,
                fromDate: action.payload,
            };
        default:
            return {
                ...state,
            };
    }
};

export const COTReportsPage = () => {
    const [state, setState] = useReducer(reducer, {
        reportedInstruments: [],
        report: {
            instrumentGuid: "",
            instrumentName: "",
            datePublished: "",
            assetManagersLong: "",
            assetManagersShort: "",
            leveragedFundsLong: "",
            leveragedFundsShort: "",
            assetManagersLongChange: "",
            assetManagersShortChange: "",
            leveragedFundsLongChange: "",
            leveragedFundsShortChange: "",
        },
        fromDate: "",
    });

    useEffect(() => {
        getReportedInstruments().then((instruments) => {
            var lastTuesday = getLastDayOFWeekDate(2);
            setState({
                type: "loadReportedInstruments",
                payload: instruments,
            });
            setState({
                type: "changeReport",
                payload: instruments[0],
            });
            setState({
                type: "setFromDate",
                payload: dayjs(lastTuesday),
            });
            getReport(
                instruments[0].guid,
                instruments[0].instrumentName,
                lastTuesday.toJSON()
            ).then((result) => {
                setState({
                    type: "loadReport",
                    payload: result,
                });
            });
        });
    }, []);

    const cotDateFromChange = (newValue) => {
        newValue = newValue.add(1, "days");
        getReport(state.report.instrumentGuid, state.report.instrumentName, newValue.toJSON()).then(
            (result) => {
                setState({
                    type: "loadReport",
                    payload: result,
                });
            }
        );
    };

    const OnChangeAssetType = (value) => {
        var instrument = state.reportedInstruments[value];

        setState({ type: "changeReport", payload: instrument });
        getReport(instrument.guid, instrument.instrumentName, state.fromDate).then((result) => {
            setState({
                type: "loadReport",
                payload: result,
            });
        });
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.headerContainer}>
                <h1>Welcome to the COT Report Tool</h1>
                <h4>Find what is the sentiment among the institutional traders</h4>
            </div>

            <div className={styles.cotInformationContainer}>
                <div className={styles.filtersContainer}>
                    <div className={styles.assetTypeContainer}>
                        <h4 className={styles.assetTypeHeading}>Asset</h4>
                        <select
                            className={styles.selectAsset}
                            onChange={(e) => OnChangeAssetType(e.target.value)}
                            select={state.report.instrumentName}
                        >
                            {state.reportedInstruments.map((futureAsset, index) => (
                                <option
                                    className={styles.selectAssetOption}
                                    key={futureAsset.guid}
                                    value={index}
                                >
                                    {futureAsset.instrumentName}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className={styles.datePickerContainer}>
                        <h4>From</h4>
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DatePicker
                                className={styles.datePicker}
                                onChange={cotDateFromChange}
                                value={state.fromDate}
                                PopperProps={{
                                    sx: popperSx,
                                }}
                                renderInput={(params) => (
                                    <CssTextField
                                        {...params}
                                        helperText={params?.inputProps?.placeholder}
                                    />
                                )}
                                views={["year", "month", "day"]}
                                showDaysOutsideCurrentMonth={true}
                                disableFuture={true}
                                shouldDisableDate={(dateParam) => {
                                    return dateParam.day() !== 2;
                                }}
                            />
                        </LocalizationProvider>
                    </div>
                </div>
            </div>
            <div className={styles.cotPositionsContainer}>
                <div className={styles.assetManagersContainer}>
                    <h2 className={styles.participantsHeader}>Asset Managers/Institutionals</h2>
                    <div className={styles.assetManagersPositions}>
                        <div className={styles.longShortContainer}>
                            <h4>Longs: </h4>
                            <h3 className={styles.assetManagersLongs}>
                                {state.report.assetManagersLong}
                            </h3>
                            <h2
                                className={classnames(
                                    styles.assetManagersLongsChange,
                                    state.report.assetManagersLongChange < 0
                                        ? styles.backgroundColorRed
                                        : styles.backgroundColorGreen
                                )}
                            >
                                {plusInFrontOfPositiveNumber(state.report.assetManagersLongChange)}
                            </h2>
                        </div>
                        <div className={styles.longShortContainer}>
                            <h4>Shorts: </h4>
                            <h3 className={styles.assetManagersShorts}>
                                {state.report.assetManagersShort}
                            </h3>
                            <h2
                                className={classnames(
                                    styles.assetManagersShortsChange,
                                    state.report.assetManagersShortChange < 0
                                        ? styles.backgroundColorRed
                                        : styles.backgroundColorGreen
                                )}
                            >
                                {plusInFrontOfPositiveNumber(state.report.assetManagersShortChange)}
                            </h2>
                        </div>
                    </div>
                </div>
                <div className={styles.leveragedFundsContainer}>
                    <h2 className={styles.participantsHeader}>Leveraged Funds</h2>
                    <div className={styles.leveragedFundsPositions}>
                        <div className={styles.longShortContainer}>
                            <h4>Longs: </h4>
                            <h3 className={styles.leveragedFundsLongs}>
                                {state.report.leveragedFundsLong}
                            </h3>
                            <h2
                                className={classnames(
                                    styles.leveragedFundsLongsChange,
                                    state.report.leveragedFundsLongChange < 0
                                        ? styles.backgroundColorRed
                                        : styles.backgroundColorGreen
                                )}
                            >
                                {plusInFrontOfPositiveNumber(state.report.leveragedFundsLongChange)}
                            </h2>
                        </div>
                        <div className={styles.longShortContainer}>
                            <h4>Shorts: </h4>
                            <h3 className={styles.leveragedFundsShorts}>
                                {state.report.leveragedFundsShort}
                            </h3>
                            <h2
                                className={classnames(
                                    styles.leveragedFundsShortsChange,
                                    state.report.leveragedFundsShortChange < 0
                                        ? styles.backgroundColorRed
                                        : styles.backgroundColorGreen
                                )}
                            >
                                {plusInFrontOfPositiveNumber(
                                    state.report.leveragedFundsShortChange
                                )}
                            </h2>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};
