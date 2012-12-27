
/*
 * GET home page.
 */

exports.index = function(req, res){
  res.render('index', { title: 'What to do when you are overloaded...' })
};