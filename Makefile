publish:
	docker build -t $(REGISTRY_URI)/redes/migrations-credenciamentos:$(GO_PIPELINE_LABEL) .
	docker push $(REGISTRY_URI)/redes/migrations-credenciamentos:$(GO_PIPELINE_LABEL)