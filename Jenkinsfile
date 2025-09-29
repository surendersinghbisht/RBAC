pipeline {
    agent any

    environment {
        DOCKER_IMAGE = "surendersinghb/rbac:latest"
        DOCKER_REGISTRY = "https://index.docker.io/v1/"
        DOCKER_CREDENTIALS = "dockerhub-credential" // This is the Jenkins credential ID
    }

    stages {
        stage('Checkout SCM') {
            steps {
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
                    docker.withRegistry("${DOCKER_REGISTRY}", "${DOCKER_CREDENTIALS}") {
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
