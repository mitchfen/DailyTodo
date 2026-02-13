#!/bin/sh

# Path to appsettings.json
CONFIG_FILE="/usr/local/webapp/nginx/html/appsettings.json"

# Check if jq is installed
if ! command -v jq &> /dev/null; then
    echo "jq is not installed. Skipping config substitution."
else
    # Check for DAILY_TASKS environment variable
    if [ -n "$DAILY_TASKS" ]; then
        echo "Found DAILY_TASKS environment variable. Parsing..."
        
        # Split by comma and create JSON array
        # map(select(length > 0)) removes empty strings
        TASKS=$(jq -n --arg v "$DAILY_TASKS" '$v | split(",") | map(select(length > 0))')
        
        echo "Updating DailyTasks in appsettings.json..."
        # Create temp file
        TMP_FILE=$(mktemp)
        jq --argjson tasks "$TASKS" '.DailyTasks = $tasks' "$CONFIG_FILE" > "$TMP_FILE" && mv "$TMP_FILE" "$CONFIG_FILE"
    else
        echo "DAILY_TASKS environment variable not found. Using default appsettings.json."
    fi
fi

# Execute the passed command
exec "$@"
