git filter-branch --force --env-filter "
if [ \"\$GIT_COMMIT\" = \"db50957aba593e0470e036449a9376b42b81da8e\" ]; then
    export GIT_AUTHOR_DATE=\"2024-06-06T19:00:00\"
    export GIT_COMMITTER_DATE=\"2024-06-06T19:00:00\"
fi
" -- --development