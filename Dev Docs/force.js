"use strict";

const parameters = new URLSearchParams(window.location.search);
const graphWidth = 1920, graphHeight = 1080, graphScale = 3;

const makeParents = parameters.has('makeparents');

(async () => {
	//  Graph data
	let graph = await d3.json('bits.json');

	let nodes = [];
	let links = [];

	function namespace(nodeId, nodeType)
	{
		return `${nodeType}::${nodeId}`;
	}

	function iterateNode(node, attrs, parentNode, graph, nodeType)
	{
		attrs = Object.assign({}, attrs);

		if ('color' in node)
			attrs.color = node.color;
		
		if ('id' in node && (makeParents || !('nodes' in node)))
		{
			nodes.push(Object.assign({ 'id':namespace(node.id, nodeType), 'name':node.id, 'isParent':'nodes' in node }, attrs));

			if (parentNode !== null && makeParents)
			 	links.push({ source: parentNode.id, target: node.id });
		}

		['bits', 'grists', 'members'].forEach(sourceType =>
		{
			if (sourceType in node)
				node[sourceType].forEach(source => links.push({ source:namespace(source, sourceType), target:namespace(node.id, nodeType) }));
		});

		if ('nodes' in node)
			node.nodes.forEach(child => iterateNode(child, attrs, 'id' in node ? node : parentNode, graph, nodeType === null ? child.id : nodeType));
	}

	iterateNode(graph, {}, null, graph, null);

	//  SVG elements
	let svg = d3.select('#graph').append('svg')
		.classed('svg', true)
		.attr('viewBox', [0, 0, graphWidth * graphScale, graphHeight * graphScale])
		.attr('width', graphWidth)
		.attr('height', graphHeight);

	let markers = [];
	[true, false].forEach(sourceParent =>
	[true, false].forEach(targetParent =>
		markers.push({
			sourceParent: sourceParent,
			targetParent: targetParent,
		})
	));
	
	const markerBoxSize = 20;
	svg.append('defs').selectAll('marker')
		.data(markers)
		.enter()
		.append('marker')
			.attr('id', marker => 'arrow' + (marker.sourceParent ? '-sourceParent' : '') + (marker.targetParent ? '-targetParent' : ''))
			.attr('viewBox', [0, 0, markerBoxSize, markerBoxSize])
			.attr('refX', marker => markerBoxSize / 2 + (marker.targetParent ? 5 : 10))
			.attr('refY', markerBoxSize / 2)
			.attr('markerWidth', markerBoxSize)
			.attr('markerHeight', markerBoxSize)
			.attr('orient', 'auto-start-reverse')
		.append('path')
			.attr('d', `M ${markerBoxSize / 2} ${markerBoxSize / 2} 0 ${markerBoxSize / 4} 0 ${markerBoxSize * 3 / 4} ${markerBoxSize / 2} ${markerBoxSize / 2}`)
			.attr('fill', marker => marker.sourceParent ? '#00000010' : '#000000')

	let link = svg.selectAll('.link')
		.data(links)
		.join('line')
		.classed('link', true);

	let node = svg.selectAll('.node')
		.data(nodes)
		.join('g')
		.classed('node', true);
	
	node.append('circle')
		.attr('r', node => node.isParent ? 5 : 10)
		.style('fill', node => node.color)
		.append('title')
			.text(node => node.name);
	
	node.append('text')
		.text(node => node.name)
		.attr('dx', 0)
		.attr('dy', node => node.isParent ? -5 : -10)
		.attr('text-anchor', 'middle');


	//  Interaction events
	function tick()
	{
		link
			.attr('x1', link => link.source.x)
			.attr('y1', link => link.source.y)
			.attr('x2', link => link.target.x)
			.attr('y2', link => link.target.y);

		node
			.attr('transform', node => `translate(${node.x}, ${node.y})`)
			.select('circle')
				.style('stroke', node => node.pinned ? '#000' : null);
	}


	//  Simulation
	var simulation = d3.forceSimulation(nodes)
		.force('charge', d3.forceManyBody().strength(-50))
		//.force('centerX', d3.forceX(graphWidth * graphScale / 2))
		//.force('centerY', d3.forceY(graphHeight * graphScale / 2))
		.force('center', d3.forceCenter(graphWidth * graphScale / 2, graphHeight * graphScale / 2))
		.force('link', d3.forceLink(links).id(node => node.id).distance(20))
		.on('tick', tick);

	node
		.call(d3.drag()
			.on('start', (event, node) => {
				if (!event.active) simulation.alphaTarget(0.3).restart();
				node.fx = node.x;
				node.fy = node.y;
			})
			.on('drag', (event, node) => {
				node.fx = event.x;
				node.fy = event.y;
			})
			.on('end', (event, node) => {
				if (!event.active) simulation.alphaTarget(0);
				if (!node.pinned)
				{
					node.fx = null;
					node.fy = null;
				}
			}))
		.on('click', (event, node) => {
			simulation.restart();
			node.pinned = !node.pinned;
			if (node.pinned)
			{
				node.fx = node.x;
				node.fy = node.y;
			}
			else
			{
				node.fx = null;
				node.fy = null;
			}
		});
	
	
	//  Final attribute and style calls (post forceLink)
	link
		.classed('parent', link => link.source.isParent)
		.attr('marker-end', link => 'url(#arrow' + (link.source.isParent ? '-sourceParent' : '') + (link.target.isParent ? '-targetParent' : '') + ')');

	node
		.classed('parent', node => node.isParent)
		.each(node => node.weight = link.filter(link => link.source === node || link.target === node).size());

	simulation.force('link')
		.strength(link => (link.source.isParent ? 0.2 : 1) / Math.min(link.source.weight, link.target.weight))
		.distance(link => link.source.isParent ? 50 : 20);
})();

// Extra notes:
// Some grists are dropped out of the price depending on the item. Themes' grists are never dropped.
// Calculate a "usefulness" rating that determines the tier of similar grists and their amounts
// Bits are combob'd. Themes are appended.
// Some themes overwrite other themes. Eg, felt overwrites red plush
// Thematic items are applied with &&
// You can make strife specibi for anything, even singular weapons and themes. you just have to find the right bits ;3
// Themed furnature only costs build unless it's useful or has information
// Size determines build grist cost, usefulness determines everything else
// Looks like any mobile computing requires diamond
// Johnny 5 is a very *unique* object :3
// WOAH punching a card with a thematic item and using it applies themes!!!