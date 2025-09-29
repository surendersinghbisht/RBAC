pipeline {
    agent any

    environment {
        // Docker image name and tag
        DOCKER_IMAGE = "surendersinghb/rbac:latest"
        DOCKER_REGISTRY = "docker.io" 
        // Jenkins credential ID for Docker Hub (username + PAT)
        DOCKER_CREDENTIALS = "dockerhub-credential" 
    }

    stages {
        stage('Checkout SCM') {
            steps {
                // No credentials needed for public repo
                git branch: 'master', url: 'https://github.com/surendersinghbisht/RBAC.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Run Tests') {
            steps {
                bat 'dotnet test --no-build --verbosity normal'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o out'
            }
        }

        stage('Build Docker Image') {
            steps {
                bat "docker build -t ${DOCKER_IMAGE} ."
            }
        }

        stage('Push Docker Image') {
            steps {
                script {
                    // Push Docker image using Jenkins Docker credentials
                    docker.withRegistry("https://${DOCKER_REGISTRY}", "${DOCKER_CREDENTIALS}") {
                        bat "docker push ${DOCKER_IMAGE}"
                    }
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed. Check the logs!'
        }
    }
}
