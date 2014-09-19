using System;

namespace BEAR.cas
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class VariablesCAS
    {
        public static String ERROR_LOG_FILE_NAME = "\\\\bmelite\\elite\\work\\logs\\BEAR\\CAS.log";  ///<Error log file location

        public static String TOOLTIP_TEXTBOXOFFICES = "Enter office locations numbers separated by a comma or a semi-colon.<br>Or, select the offices from the list on the right."; /**< Passed to JavaScript for tool tip*/
        public static String TOOLTIP_RESIZE_COLUMNS = "Click to reset columns to<br>default column widths."; /**< Passed to JavaScript for tool tip*/
        public static String TOOLTIP_SEARCH_EXECUTE = "Click here or press Enter to Search"; /**< Passed to JavaScript for tool tip*/

        public static int NUMBER_ROWS_ON_FINAL_PAGE_TO_KEEP_FIXED_PAGERROW = 6;  /**< if there are this many or less rows on the final page, then remove the fixed footer css class on the pager row*/

        public static string[] VALID_MATTER_STATUS = new string[2] {"OC", "OP"};  /**< used to validate matters in UtilityCAS.cs */

    }
}
