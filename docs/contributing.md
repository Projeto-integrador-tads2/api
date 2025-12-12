# Guia de Contribuição - CRM API

> Última atualização: 2025-12-03
> Versão do documento: 1.0

## Índice
- [Fluxo de trabalho](#fluxo-de-trabalho)
- [Convenções de commits](#convenções-de-commits)
- [Ambiente de desenvolvimento](#ambiente-de-desenvolvimento)
- [Processo de revisão e aprovação](#processo-de-revisão-e-aprovação)
- [Código de conduta](#código-de-conduta)
- [Checklist de revisão PR](#checklist-de-revisão-pr)
- [Referências](#referências)
- [TODOs / Perguntas](#todos--perguntas)

## Fluxo de trabalho
- Branches: `feature/`, `bugfix/`, `hotfix/`, `release/`
- PRs: Descrição clara, checklist de testes, link para issue
- Requisitos para PR: passar linter, testes, revisão
- Política: trunk-based (merge em `development`), depois `main`

## Convenções de commits
- [Conventional Commits](https://www.conventionalcommits.org/)
  - Exemplo: `feat(auth): adicionar endpoint de registro`
- Mensagens curtas e objetivas

## Ambiente de desenvolvimento
- Subir ambiente com `docker-compose up -d`
- Rodar API local com `dotnet run` ou `start.sh`
- Usar `appsettings.Development.json` para configs locais

## Processo de revisão e aprovação
- PRs revisados por pelo menos 1 maintainer
- Checklist de revisão:
  - Código limpo e documentado
  - Testes passando
  - Sem segredos expostos
  - Descrição clara

## Código de conduta
- Respeito e colaboração
- Comunicação clara
- Sem discriminação
- TODO: Adicionar link para código de conduta

## Checklist de revisão PR
- [ ] Código segue padrões do projeto
- [ ] Testes unitários/integrados passam
- [ ] Sem segredos ou dados sensíveis
- [ ] Documentação atualizada
- [ ] Descrição clara do PR

## Referências
- [README.md](../../README.md)
- [docker-compose.yml](../docker-compose.yml)
- [appsettings.Development.json](../appsettings.Development.json)

## TODOs / Perguntas
- Adicionar PR template ao repositório
- Detalhar processo de aprovação
- Confirmar canal de comunicação oficial
