# Drop Editor EO

Editor de drops para servidores privados de Eudemons Online.
Permite gerenciar a chain de acoes (cq_action) vinculadas a monstros e itens das tabelas cq_monstertype e cq_itemtype.

## Funcionalidades

- **Modo Monstros:** Lista e filtra monstros da tabela cq_monstertype
- **Modo Itens:** Lista e filtra itens da tabela cq_itemtype
- **Chain de Acoes:** Visualiza e edita a cadeia de acoes vinculada ao monstro/item selecionado
- **Filtro de Itens:** Preview de itens com busca por nome e copia rapida do ID
- **Exportar SQL:** Gera script SQL das alteracoes realizadas na sessao
- **AutoComplete:** Sugestoes automaticas ao digitar nomes de monstros ou itens

## Como Usar

1. Conecte ao banco MySQL na tela inicial
2. Selecione o modo (Monstros ou Itens)
3. Digite um nome no filtro e pressione Enter ou clique em Buscar
4. Selecione um registro para ver sua chain de acoes
5. Edite a chain diretamente na grade
6. Use "Adicionar Acao" / "Excluir Acao" para modificar a cadeia
7. "Copiar ID" no filtro de itens para usar IDs na edicao da chain
8. Exporte o SQL das alteracoes com "Exportar SQL"

## Suporte

Desenvolvido por Erik Martins
