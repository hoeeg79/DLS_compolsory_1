workspace {
    model {
        User = person "User" "User of the HelloPlanet application"
        
        SearchEnronFiles = softwareSystem "Search System" "A web application that allows user to search the Enron emails." {
            WebApp = container "Web Application" "User interface for searching the data" "Angular"
            
            DataCleaner = container "Cleaning Service" "Service for loading and cleaning the emails. It is to this one you need to provide the files." "ASP.NET Core Web API" {
                CleanerController = component  "Cleaner Controller" "Handles API requests for files (emails)."
                CleanerRepository = component "Cleaner Repository" "Handles calls to DB API for storage of cleaned files."
            }
            
            SearchService = container "Search Service" "Provides search results for queries into the files." "ASP.NET Core Web API" {
                SearchController = component "Search Controller" "Handles API requests for searches."
                SearchRepository = component "Search Repository" "Makes calls to DB API for searches."
            }
            
            DatabaseService = container "Database Service" "Provides a random greeting based on the language." "ASP.NET Core Web API" {
                DatabaseController = component "Database Controller" "Handles API requests for DB queries."
                DatabaseRepository = component "Database Repository" "Handles queries to DB."
            }
        }
        
        // Relationships
        User -> WebApp "Interacts with"
        WebApp -> DataCleaner "Uploads email files"
        WebApp -> SearchService "Search threw data"
        DataCleaner -> DatabaseService "Gives cleaned data"
        SearchService -> DatabaseService "Queries for a search"

        CleanerController -> CleanerRepository "Uses"
        SearchController -> SearchRepository "Uses"
        DatabaseController -> DatabaseRepository "Uses"
    }

    views {
        styles {
            element "Element" {
                color #ffffff
            }
            element "Person" {
                background #9b191f
                shape person
            }
            element "Software System" {
                background #ba1e25
            }
            element "Database" {
                shape cylinder
            }
            element "Container" {
                background #d9232b
            }
            element "Component" {
                background #E66C5A
            }
            element "WebBrowser" {
                shape WebBrowser
            }
        }
    }
}